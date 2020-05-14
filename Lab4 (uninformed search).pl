%% Goal states
goal([3, 0]).
goal([0, 3]).

%% Main function
main(Decision) :-
    depth(0, [0, 0], []).

%% Depth-limited search (recursion-based)
depth(_, State, _) :-
    goal(State).
depth(Level, State, History) :-
    after(Level, State, State1),
    not(member(State1, History)),
    NextLevel is Level+1,
    depth(NextLevel, State1, [State1 | History]),
    writeln(State1).

%% Contains all the descriptions of eligibility of a state
%% Head is a 9 L bucket, Tail is a 5 L bucket
after(Level, [FirstStateHead | [FirstStateTail | _]], [SecondStateHead | [SecondStateTail | _]]) :-
    Level < 20, FirstStateHead = SecondStateHead, SecondStateTail = 0;
    Level < 20, FirstStateTail = SecondStateTail, SecondStateHead = 0;
    Level < 20, FirstStateHead = SecondStateHead, SecondStateTail = 5;
    Level < 20, FirstStateTail = SecondStateTail, SecondStateHead = 9;
    Level < 20, SecondStateHead = 0, SecondStateTail is FirstStateHead + FirstStateTail - SecondStateHead, SecondStateTail =< 5, SecondStateTail >= 0;
    Level < 20, SecondStateTail = 0, SecondStateHead is FirstStateHead + FirstStateTail - SecondStateTail, SecondStateHead =< 9, SecondStateHead >= 0;
    Level < 20, SecondStateHead = 9, SecondStateTail is FirstStateHead + FirstStateTail - SecondStateHead, SecondStateTail =< 5, SecondStateTail >= 0;
    Level < 20, SecondStateTail = 5, SecondStateHead is FirstStateHead + FirstStateTail - SecondStateTail, SecondStateHead =< 9, SecondStateHead >= 0.