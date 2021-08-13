module Termination 

let rec length (#a:Type) (l: list a) 
    : Tot nat (decreases l)
    = match l with
      | [] -> 0
      | _::t -> 1 + length t

// val append : (#a:Type) -> (l1: list a) -> (l2: list a) -> (l3: list a{length l3 = length l1 + length l2})
val append (#a:Type) (l1 l2: list a)
    : l:list a{length l = length l1 + length l2}
let rec append l1 l2 
    = match l1 with 
      | [] -> l2
      | h::t -> h :: append t l2  

(*  let's prove that the ackerman function terminates
    we have two inputs m & n 
    we annotate the function Type using decreases on 
    the lexicographic ordering of the params  *)
let rec ack (m n:nat) :Tot nat (decreases %[m;n]) 
    = if m = 0 then n + 1
      else if n = 0 then ack (m - 1) 1
      else ack (m - 1) (ack m (n - 1)) 


(*  innefficient fibb
    termination proof is the decrease of the input n *) 
let rec fibonacci_slow (n:nat) : Tot nat (decreases n)
    = if n <= 1 then 1 
      else fibonacci_slow (n - 1) + fibonacci_slow (n - 2)

(*  efficient fibb 
    termination proof is the decrease of n  *)
let rec fibonacci_fast (a b n:nat): Tot nat (decreases n)
    = match n with 
      | 0 -> a
      | _ -> fibonacci_fast b (a + b) (n - 1)


//  innefficient rev
let rec rev_ineff #a (l:list a) : list a 
    = match l with
      | [] -> []
      | h::t -> append (rev_ineff t) [h]

(*  efficient rev 
    termination proof is the decrease of l  *)
//  l = cons h tail => tail << l so l decreases
let rec rev_eff #a (acc l:list a) 
    :Tot (list a) (decreases l)
    =   match l with 
        | [] -> acc
        | h::t -> rev_eff (h::acc) t
