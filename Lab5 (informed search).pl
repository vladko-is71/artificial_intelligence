goal([white, white, white, empty, black, black, black, black, _, _]).

main(Initial) :-
    astar([Initial], Solution),
    get_value(Solution, 8, Length),
    get_value(Solution, 9, Path),
    read_path_backwards(Path),
    write('The path length was '),
    writeln(Length).

%% This function helps to read the path
%% (because the transitions are written in reversed order)
read_path_backwards([Head]) :-
    writeln(Head).
read_path_backwards([Head | Tail]) :-
    read_path_backwards(Tail),
    writeln(Head).

%% AStar algorithm
astar(List, Solution) :-
    List = [BestElement | _],
    goal(BestElement),
    Solution = BestElement.
astar(OldList, Solution) :-
    OldList = [BestElement | FutureOldList],
    get_possible_moves(BestElement, FutureOldList, NewList),
    insertion_sort(NewList, SortedList),
    astar(SortedList, ReturnedSolution),
    Solution = ReturnedSolution.

%% Returns all possible moves from the said state
get_possible_moves(State, CurrentList, Answer) :-
    (black_to_right_once(State, NewStateX) -> XList = [NewStateX | CurrentList]; XList = CurrentList),
    (white_to_left_once(State, NewStateY) -> YList = [NewStateY | XList]; YList = XList),
    (black_to_right_twice(State, NewStateZ) -> ZList = [NewStateZ | YList]; ZList = YList),
    (white_to_left_twice(State, NewStateA) -> AList = [NewStateA | ZList]; AList = ZList),
    Answer = AList.

%% Predicates to generate the possible transitions from the current state
black_to_right_once(State, NewStateFinalView) :-
    choose_empty(State, EmptyCoordinate),
    EmptyCoordinate >= 1, EmptyCoordinate =< 7,
    ProbableBlackCoordinate is EmptyCoordinate - 1,
    get_value(State, ProbableBlackCoordinate, black),
    swap_neighbours_anywhere(State, NewState, ProbableBlackCoordinate),
    set_last_values(NewState, NewStateFinalView, EmptyCoordinate-ProbableBlackCoordinate).
white_to_left_once(State, NewStateFinalView) :-
    choose_empty(State, EmptyCoordinate),
    EmptyCoordinate >= 0, EmptyCoordinate =< 6,
    ProbableWhiteCoordinate is EmptyCoordinate + 1,
    get_value(State, ProbableWhiteCoordinate, white),
    swap_neighbours_anywhere(State, NewState, EmptyCoordinate),
    set_last_values(NewState, NewStateFinalView, EmptyCoordinate-ProbableWhiteCoordinate).
black_to_right_twice(State, NewStateFinalView) :-
    choose_empty(State, EmptyCoordinate),
    EmptyCoordinate >= 2, EmptyCoordinate =< 7,
    ProbableBlackCoordinate is EmptyCoordinate - 2,
    get_value(State, ProbableBlackCoordinate, black),
    swap_through_anywhere(State, NewState, ProbableBlackCoordinate),
    set_last_values(NewState, NewStateFinalView, EmptyCoordinate-ProbableBlackCoordinate).
white_to_left_twice(State, NewStateFinalView) :-
    choose_empty(State, EmptyCoordinate),
    EmptyCoordinate >= 0, EmptyCoordinate =< 5,
    ProbableWhiteCoordinate is EmptyCoordinate + 2,
    get_value(State, ProbableWhiteCoordinate, white),
    swap_through_anywhere(State, NewState, EmptyCoordinate),
    set_last_values(NewState, NewStateFinalView, EmptyCoordinate-ProbableWhiteCoordinate).

%% Insertion sort functions (http://kti.ms.mff.cuni.cz/~bartak/prolog/sorting.html)
%% Used for making a list look like a priority queue
insertion_sort(List, Sorted) :-
    i_sort(List, [], Sorted).
i_sort([], Accumulator, Accumulator).
i_sort([Head | Tail], Accumulator, Sorted) :-
    insert(Head, Accumulator, NewAccumulator),
    i_sort(Tail, NewAccumulator, Sorted).
%% inserting elements to list
insert(Head, [SubHead | Tail], [SubHead | NewTail]) :-
    state_cost(Head, X), state_cost(SubHead, Y),
    X > Y,
    insert(Head, Tail, NewTail).
insert(Head, [SubHead | Tail], [Head, SubHead | Tail]) :-
    state_cost(Head, X), state_cost(SubHead, Y),
    X =< Y.
insert(Element, [], [Element]).

%% Returns the value which is used for sorting (making the priority queue):
%% number of already passed transitions + heuristics
state_cost(State, Cost) :-
    get_value(State, 8, InitialCost),
    heuristics(State, [white, white, white, empty, black, black, black, black, _, _], 0, HeuristicCost),
    Cost is InitialCost + HeuristicCost.

%% Getting the value in the list:
%% in imperative languages, it would look like comparing State[Counter] and Value
get_value(State, Counter, Value) :-
    Counter = 0,
    State = [Value | _].
get_value(State, Counter, Value) :-
    State = [_ | Tail],
    get_value(Tail, NewCounter, Value),
    Counter is NewCounter + 1.

%% Penultimate element of a list representing a state is a number of transitions;
%% after each transition, while generating a new state, it should be increased by 1
%% Last element of a list represents a list of all previous transitions
set_last_values([LastElement, Queue], [ChangedState, NewQueue], OldElement) :-
    ChangedState is LastElement + 1,
    NewQueue = [OldElement | Queue].
set_last_values(State, ChangedState, OldElement) :-
    State = [Head | Tail],
    set_last_values(Tail, TailOfChangedState, OldElement),
    ChangedState = [Head | TailOfChangedState].

%% Getting the coordinate of empty slot in State list
choose_empty(State, Counter) :-
    State = [Head | _],
    Head = empty,
    Counter = 0.
choose_empty(State, Counter) :-
    State = [_ | Tail],
    choose_empty(Tail, NewCounter),
    Counter is NewCounter + 1.

%% Heuristic function of a state
heuristics([_], [_], _, Function) :-
    Function is 0.
heuristics([Head | Tail], [GoalHead | GoalTail], Level, Function) :-
    Head = GoalHead,
    NextLevel is Level + 1,
    heuristics(Tail, GoalTail, NextLevel, Function).
heuristics([Head | Tail], [GoalHead | GoalTail], Level, Function) :-
    Head \= GoalHead,
    NextLevel is Level + 1,
    heuristics(Tail, GoalTail, NextLevel, PreviousFunction),
    Function is PreviousFunction + 1.

%% Predicates for swapping the elements of a list:
%% we can swap either neighbour elements or those that have one element between them
swap_neighbours([X, Y | Tail], [Y, X | Tail]).
swap_through([X, Y, Z | Tail], [Z, Y, X | Tail]).

%% Predicates for swapping the elements of a list:
%% handling the case if two swapped elements are not in the beginning of the list
swap_neighbours_anywhere(State, NewStateEnd, EarlierLevel) :-
    EarlierLevel = 0,
    swap_neighbours(State, NewStateEnd).
swap_neighbours_anywhere(State, NewState, EarlierLevel) :-
    State = [StateHead | StateTail],
    NewLevel is EarlierLevel - 1,
    swap_neighbours_anywhere(StateTail, NewStateEnd, NewLevel),
    NewState = [StateHead | NewStateEnd].
swap_through_anywhere(State, NewStateEnd, EarlierLevel) :-
    EarlierLevel = 0,
    swap_through(State, NewStateEnd).
swap_through_anywhere(State, NewState, EarlierLevel) :-
    State = [StateHead | StateTail],
    NewLevel is EarlierLevel - 1,
    swap_through_anywhere(StateTail, NewStateEnd, NewLevel),
    NewState = [StateHead | NewStateEnd].