module Basics
open FStar.Mul
open FStar.Real
//==================================================================================================

let ( |> ) (x:'a) (f:'a->'b) = f x
let ( >> ) (f:'a->'b) (g:'b->'c) (x:'a) = g (f x)
let rec ( -- ) (low:nat) (high:nat{ high >= low}) 
                : Tot (list nat) (decreases %[high - low]) 
                =   if low = high then [] 
                    else low::((low + 1) -- high) 
let rec map (f:nat->nat)  (l:list nat) = match l with
    | []    -> []
    | h::t  -> (f h) :: (map f t) 
//==================================================================================================
type divides  (divisor :nat) (n:nat) = exists (k:nat). k*divisor=n
type isPrime n = ~(exists (d:nat). divides d n /\ 1<d /\ d<n) /\ n>1

let _ = assert(isPrime 3)
let _ = assert(isPrime 23)

[@@expect_failure] let _ = assert(isPrime 4) //does not pass the Z3 SMT Solver's query
//==================================================================================================
let ( / ) (k:nat) (n:nat) :Type = ∃ m. m*k = n 
let   p   (p:nat{p > 1} ) :Type = ∀ d. ~(d/p) \/ 1>=d \/ d>=p

let _ = assert( p 23 /\ p 2 /\ p 3)

[@@expect_failure] let _ = assert(p 1) //does not pass the Z3 SMT Solver's query
//==================================================================================================
let r = (1 -- 24) |> (map (λ i -> 23))
//==================================================================================================
val max : x:int -> y:int -> z:int{ z >= x && z >= y }
val min : x:nat -> y:nat -> z:nat{ z = x  || z = y  }

let max x y = if x > y then x else y
let min x y = if x < y then x else y

let _ = assert (forall x y. max x y = min x y ==> x = y)

let _ = assert (forall x y. max x y >= x /\
                            max x y >= y /\
                            (max x y = x \/ max x y = y))

let _ = assert (forall x y. min x y <= x /\ 
                            min x y <= y /\
                            (min x y = x \/ min x y = y))
//==================================================================================================
type favorite (n:nat) = n = 23

let _ = assert(favorite 23)
[@@expect_failure] let _ = assert(favorite 22) //doesn't pass cause cleary 22 sucks as a number


//==================================================================================================
let _ = assert (forall (x:nat). 0 <= x)

let _ = assert (forall (x:int{x < 0}). x < 0)

let _ = assert (forall (n:nat) (m:nat).
                    (m + n = 0 ==> m = 0 /\ n = 0) /\ 
                    (m * n = 0 ==> m = 0 \/ n = 0))

//===========================================================================================================
val fac : x:nat -> z:nat
let rec fac n = match n with 
    | 0 -> 0
    | n -> n * fac (n-1)

val fibb : i:nat -> r:nat{ r >= 1 /\ r >= i }
let rec fibb n = match n with 
    | 0 | 1 -> 1
    | _ -> fibb (n-1) + fibb (n-2)

//==================================================================================================
//type lowBound = forall (n:nat). exists (m:nat). m <= n
type lowBound  = exists (m:nat). forall (n:nat). m <= n

//==================================================================================================
val pow2 : x:real -> y:real{ y >=. zero } 
let pow2 x = x *. x

//==================================================================================================
let pow_assert1 x = 
    assume (x <> 0);
    assert (x * x > 0)

(*let pow_assert2 x =assume (x <> 0);   //  evaluates to wrong /false
                    assert (x * x > 0) //  cause clearly sqr func 
                    assert (x * x < 0) //  is a map from R to R+*)

//==================================================================================================