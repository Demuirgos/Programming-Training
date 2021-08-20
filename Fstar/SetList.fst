module SetList
open Quicksort

type match_head (#t:eqtype) (l1:list t) (l2:list t) =
  (l1 = [] /\ l2 = []) \/
  (exists h t1 t2. l1 = h::t1 /\ l2 = h::t2)

val dedup: #t:eqtype -> f: (t -> t -> Tot bool)
                     -> l:list t{is_sorted f l} 
                     -> Tot (r:list t{is_sorted f r /\ (forall i. is_member_of i l = is_member_of i r) /\ match_head l r})
let rec dedup #t f l = match l with
    | [] | [_] -> l
    | h::t::ts -> if h=t then dedup f (t::ts)
                  else h :: dedup f (t :: ts)

val toSet: #t:eqtype -> f: (t -> t -> Tot bool) {total_order t f} 
                      -> l:list t -> r:list t{is_sorted f r}
let toSet #t f l = dedup f (sort f l)