;initial state
(deffacts initial (case 0 0 "Initial"))

;fill the small bucket
(defrule pour-small
(case ?small ?big $?path)
(not (exists (case 5 ?big $?anypath))) =>
(assert (tempcase 5 ?big $?path "Pour small to 5")))

;fill the big bucket
(defrule pour-big
(case ?small ?big $?path)
(not (exists (case ?small 9 $?anypath))) =>
(assert (tempcase ?small 9 $?path "Pour big to 9")))

;make the small bucket empty
(defrule pour-out-small
(case ?small ?big $?path)
(not (exists (case 0 ?big $?anypath))) =>
(assert (tempcase 0 ?big $?path "Pour out small to 0")))

;make the big bucket empty
(defrule pour-out-big
(case ?small ?big $?path)
(not (exists (case ?small 0 $?anypath))) =>
(assert (tempcase ?small 0 $?path "Pour out big to 0")))

;pour water to small bucket until it becomes full
(defrule pour-to-small-to-max-temp
(case ?small ?big $?path)
(test (>= (+ ?small ?big) 5)) =>
(assert (tempcase 5 (- (+ ?small ?big) 5) $?path "Pour as much as possible to small")))

;pour water to small bucket until big bucket becomes empty
(defrule pour-to-small-to-possible-temp
(case ?small ?big $?path)
(test (< (+ ?small ?big) 5)) =>
(assert (tempcase (+ ?small ?big) 0 $?path "Pour as much as possible to small")))

;pour water to big bucket until it becomes full
(defrule pour-to-big-to-max-temp
(case ?small ?big $?path)
(test (>= (+ ?small ?big) 9)) =>
(assert (tempcase (- (+ ?small ?big) 9) 9 $?path "Pour as much as possible to big")))

;pour water to big bucket until small bucket becomes empty
(defrule pour-to-big-to-possible-temp
(case ?small ?big $?path)
(test (< (+ ?small ?big) 9)) =>
(assert (tempcase 0 (+ ?small ?big) $?path "Pour as much as possible to big")))

;adding the case if it is eligible and non-duplicate
(defrule remove-temp
(tempcase ?small ?big $?path)
(test (>= ?small 0))
(test (>= ?big 0))
(test (<= ?small 5))
(test (<= ?big 9))
(not (exists (case ?small ?big $?anypath))) =>
(assert (case ?small ?big $?path)))

;terminating the search if small bucket contains 3 L
(defrule solution-found-in-small
(case ?small ?big $?path)
(test (= ?small 3)) =>
(printout t "the solution is found in small bucket" crlf)
(printout t $?path crlf)
(halt))

;terminating the search if big bucket contains 3 L
(defrule solution-found-in-big
(case ?small ?big $?path)
(test (= ?big 3)) =>
(printout t "the solution is found in big bucket" crlf)
(printout t $?path crlf)
(halt))