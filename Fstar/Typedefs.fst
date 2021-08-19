module Typedefs
open FStar.Mul

let rec len l = match l with [] -> 0 | _::t -> 1 + len t
type myList 'a = 
    | Nil : myList 'a
    | App : hd: 'a -> tl : myList 'a -> myList 'a

let hd (l: myList 'a{App? l}) : Tot ('a) = match l with 
    | App h _ -> h 
let tl (l: myList 'a{App? l}) : Tot (myList 'a) = match l with
    | App _ t -> t

//_________________________________________________________________________________

let rec get_nth (l:list 'a{Cons? l}) (n:int{ n>=0 && n < (len l)})
    : Tot 'a (decreases l)
    = match l with 
    | h::t ->   if n = 0 then h 
                else get_nth t (n-1)
                