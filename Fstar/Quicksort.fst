module Quicksort

// typedefs : =====================================================================================================\\
type total_order (t:eqtype) (f:t->t->Tot bool) = (forall x. f x x) 
                                              /\ (forall x y. f x y /\ x <> y <==> not (f y x))
                                              /\ (forall x y z. f x y /\ f y z ==> f x z)
// prereqs : =====================================================================================================\\
val is_sorted : ('a -> 'a -> Tot bool) -> list 'a -> Tot bool
let rec is_sorted f l = match l with
    | [] | [_] -> true
    | h1::h2::t1 -> f h1 h2 && is_sorted f (h2::t1)

val append : list 'a -> list 'a -> Tot (list 'a)
let rec append l1 l2 = match l1 with [] -> l2 | h::t1 -> h::append t1 l2

let rec length (l:list 'a) 
    : Tot nat (decreases l)
    = match l with 
    | [] -> 0
    | _::t -> 1 + length t

let rec is_member_of (#t:eqtype) (i:t) (l:list t) : Tot bool = match l with 
    | [] -> false
    | h::t -> if i = h then true else is_member_of i t 

val partition: #t:eqtype -> (t -> Tot bool) -> list t -> Tot (list t * list t)
(*let partition_answer f l = 
    let rec partition_loop acc l
    : Tot (list int * list int) (decreases l)
    = match l with
    | []      -> acc
    | (h::t)  -> let a, b = acc in if f h 
                 then partition_loop (h::a, b) t
                 else partition_loop (a, h::b) t 
    in partition_loop ([],[]) l*)
    
let rec partition #t f = function
  | []      ->  [], []
  | hd::tl  ->  let l1, l2 = partition f tl in
                if f hd then hd::l1, l2 else l1, hd::l2

let rec count_occurrance (#t:eqtype) (i:t) (l:list t) 
    : Tot nat
    = match l with 
    | []    -> 0
    | h::tl -> if h <> i then (count_occurrance i tl)
               else 1 + ( count_occurrance i tl )

let is_in (#t:eqtype) (i:t) (l:list t) : Tot bool = count_occurrance i l > 0 

//================================================================================================================\\ 
val append_membership_union : #t:eqtype -> l1:list t -> l2:list t
                            -> Lemma (forall x. is_member_of x (append l1 l2) 
                                                = (is_member_of x l1 || is_member_of x l2))
                               [SMTPat (append l1 l2)]
let rec append_membership_union #t l1 l2 = match l1 with 
                            | [] -> ()
                            | _::t1 -> append_membership_union t1 l2

val partition_membership_lemma : #t:eqtype -> f:(t-> Tot bool) -> l:(list t)
    -> Lemma (ensures ( let t, b = partition f l in
                        length t + length b = length l    
                        /\ (forall x. is_member_of x t ==> f x)                  
                        /\ (forall x. is_member_of x b ==> not (f x))                  
                        /\ (forall x. is_member_of x l 
                                    = (is_member_of x t || is_member_of x b))
                        /\ (forall i. count_occurrance i l 
                                    = (count_occurrance i t) + (count_occurrance i b))))
       [SMTPat (partition f l)]
let rec partition_membership_lemma #t f l = match l with 
    | [] -> ()
    | _::ls -> partition_membership_lemma f ls

val concat_sorted_is_sorted :  #t:eqtype -> f:(t->t->Tot bool) -> l1:list t -> l2:list t -> pivot:t
                            -> Lemma (requires  ((is_sorted f l1 /\ is_sorted f l2 /\ (total_order t f))
                                            /\  (forall x. is_member_of x l1 ==> not (f pivot x))
                                            /\  (forall y. is_member_of y l2 ==> f pivot y)))
                                     (ensures (is_sorted f (append l1 (pivot :: l2))))
                               [SMTPat (is_sorted f (append l1 (pivot :: l2)))]
let rec concat_sorted_is_sorted #t f l1 l2 pivot = match l1 with
                            | [] -> ()
                            | h::ts -> concat_sorted_is_sorted f ts l2 pivot

//================================================================================================================\\ 
val sort: #t:eqtype -> f:(t -> t -> Tot bool){total_order t f}
       -> l:list t
       -> Tot (m:list t{is_sorted f m /\ (forall i. is_member_of i l = is_member_of i m)})
              (decreases (length l))

let rec sort #t f l = match l with
  | [] -> []
  | pivot::tl ->
    let hi, lo = partition (f pivot) tl in
    assert (forall i. is_member_of i (pivot :: sort f hi) 
                    = is_member_of i (append [pivot] (sort f hi)));
    append (sort f lo) (pivot::sort f hi)
