module Lemmas
open FStar.Mul

type option 'a = 
    | None 
    | Some : v:'a -> option 'a

let rec factorial (n:nat) 
    : nat
    = if n = 0 
      then 1 
      else n * factorial (n - 1)

let rec fibonacci (n:nat)
  : nat
  = if n <= 1
    then 1
    else fibonacci (n - 1) + fibonacci (n - 2)

let rec length (#a:Type) (l:list a) 
    : nat 
    = match l with
      | [] -> 0
      | _::t-> 1 + length t

let rec append (#a:Type) (l1 l2:list a)
  : list a
  = match l1 with
    | [] -> l2
    | hd :: tl -> hd :: append tl l2

let rec reverse (#a:Type) (l:list a)
  : list a 
  = match l with 
    | []   -> []
    | h::t -> append (reverse t) [h]

let cons (l:list 'a) (h:'a) = h::l
let snoc (l:list 'a) (h:'a) = append l [h]

let rec map f l = match l with 
    | [] -> []
    | h::t -> f h:: map f t

let rec foldl (f:('b -> 'a -> Tot 'a)) (a:'a) (b:list 'b) 
    : Tot 'a (decreases b)
    = match b with 
    | [] -> a
    | h::t -> foldl f (f h a) t
//====================================================================

// let's prove that (fact n) > 0 for all n:nat
(* we will use proof by induction using lemmas 
   we have fact(0) = 1 > 0 so for n = 0 : fact(n) > 0
   we suppose it's true for all n and we prove it for (n + 1)
   
   n + 1 > 0 /\ fact(n+1) = (n+1) * fact(n) /\ fact(n)>0
        ==> (n+1) * fact(n) > 0 ==> fact(n+1) > 0
        ==> for all naturals n fact(n) > 0
   let's do this F* style : )
   *)
let rec factorial_is_strictly_postive (n:nat)
    : Lemma (factorial n > 0)
    = match n with 
      | 0 -> () 
      | _ -> factorial_is_strictly_postive (n-1)

//====================================================================

// let's prove that (fact n) > n for n:nat > 2
(* we will use proof by induction using lemmas 
   we have fact(3) = 6 > 3 so for n = 3 : fact(n) > n
   we suppose it's true for all n and we prove it for (n + 1)
   
   n + 1 > 0 /\ fact(n+1) = (n+1) * fact(n) /\ fact(n) > n
        ==> (n+1) * fact(n) > (n+1) * n
        ==> fact(n+1) - (n+1) > (n+1) * (n-1)
        since n > 2 ==> n - 1 > 0 
                    ==> (n + 1) * (n - 1) > 0
                    ==> fact(n+1) - (n + 1) > 0
            ==> fact(n+1) > (n+1)
        ==> for all naturals n fact(n) > n
   let's do this F* style : )
   *)

let rec factorial_n_is_greater_than_arg (n:int)
    : Lemma ( requires n > 2)
            ( ensures  factorial n > n )
    = match n with 
      | 3 -> ()
      | _ -> factorial_n_is_greater_than_arg (n-1)   

//====================================================================

// let's prove that (fibb n) >= n for n:nat > 1
(* we will use proof by induction using lemmas 
   we have fibb(2) = 2 >= 2 so for n = 2 : fact(n) >= n
   we suppose it's true for all n and we prove it for (n + 1)
   fibb(n+1) = fibb(n) + fibb(n-1) /\ (forall m <= n. fibb(m) >= m)
        ==> fibb(n+1) = fibb(n) + fibb(n+1) >= 2*n - 1 
        since n >= 2 ==> n - 2 >= 0 ==> 2*n - 2 >= n
                     ==> 2*n - 1 >= n + 1
        ==> fibb(n+1) >= n + 1
        ==> for all naturals n fibb(n) >= n
   let's do this F* style : )
   *)

let rec fibonacci_greater_or_equal_arg (n:nat{n>=2})
    : Lemma (fibonacci n >= n)
    = if n <= 3 then ()
      else (
          fibonacci_greater_or_equal_arg (n-1);
          fibonacci_greater_or_equal_arg (n-2)
      )

//====================================================================

// let's prove that len(append(l1,l2)) = len(l1) + len(l2)

let rec append_result_length_is_sum_of_length_of_args (#a:Type) (l1 l2:list a)
    : Lemma (length (append l1 l2) = length l1 + length l2)
    = match l1 with 
      | [] -> ()
      | _::t -> append_result_length_is_sum_of_length_of_args t l2

//====================================================================

// let's prove that (rev o rev) l = l

let rec reverse_snoc_is_cons #a (l:list a) (e:a) 
    : Lemma (reverse (snoc l e) == e::(reverse l))
    = match l with 
      | []   -> ()
      | h::t -> reverse_snoc_is_cons t e

let rec reverse_of_reverse_is_Id #a (l:list a) 
    : Lemma (reverse (reverse l) == l)
    = match l with 
      | []   -> ()
      | h::t -> 
        reverse_of_reverse_is_Id t;
        reverse_snoc_is_cons (reverse t) h  

//====================================================================

// let's prove that reverse function is injective 
// e.g : (rev l1) = (rev l2) <==> l1 = l2
let rec snoc_is_injective #a (l1 l2:list a) (e1 e2:a) 
    : Lemma (requires (snoc l1 e1) == (snoc l2 e2))
            (ensures l1 == l2 /\ e1 == e2)
    = match l1, l2 with
      | _::t1, _::t2 -> snoc_is_injective t1 t2 e1 e2
      | _ -> ()
    
let rec reverse_is_injective #a (l1 l2:list a)
    : Lemma (requires reverse l1 == reverse l2)
            (ensures  l1 == l2)
    = match l1, l2 with 
      | h1::t1, h2::t2 -> 
        snoc_is_injective (reverse t1) (reverse t2) h1 h2;
        reverse_is_injective t1 t2
      | _ -> ()

//====================================================================

// let's prove that find f l = Some v ==> f v = true
val find : f:('a -> Tot bool) -> (l:list 'a) -> Tot (option (x:'a{f x}))
let rec find f l = match l with 
    | [] -> None
    | h::t -> if f h then Some h else find f t

//====================================================================

// let's prove that fold_left Cons [] == reverse
let rec append_is_associative #a (l1 l2 l3:list a) 
    : Lemma (append l1 (append l2 l3) == append (append l1 l2) l3)
    = match l1 with 
    | [] -> ()
    | _::t1 -> append_is_associative t1 l2 l3

let rec foldl_cons_is_reverse #a (l1 l2:list a) 
    : Lemma (foldl Cons l2 l1 == append (reverse l1) l2)
    = match l1 with 
    | [] -> ()
    | h1::t1 -> 
        append_is_associative (reverse t1) [h1] l2;
        foldl_cons_is_reverse t1 (h1::l2)

 