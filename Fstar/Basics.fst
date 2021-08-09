module Basics

open FStar.Mul

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

let _ = assert (forall (x:nat). 0 <= x)

let _ = assert (forall (x:int{x < 0}). x < 0)

let _ = assert (forall (n:nat) (m:nat).
                    (m + n = 0 ==> m = 0 /\ n = 0) /\ 
                    (m * n = 0 ==> m = 0 \/ n = 0))