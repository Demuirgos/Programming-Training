module Church

type list    =
    | Nil : list
    | Cons: natural -> list -> list
and natural =  
    | O   : natural
    | Suc : natural -> natural

// send refined nats to church 
let rec from (n:nat) : natural = 
    if n = 0 then O 
    else Suc (from (n-1))
let (!) = from
// gets the first elem in a list (list must not be [])
let head (l:list{~(Nil? l)}) = match l with
    | Cons hd tl -> hd
let tail (l:list{~(Nil? l)}) = match l with 
    | Cons hd tl -> tl
// get predecessor of a church nat
let pred (n:natural{Suc? n}) : natural = match n with 
    | Suc n -> n
// greater then by checking if rhs is in the 
// chained Preds of lhs
let rec gt n m = match n with 
    | O -> false
    | Suc k -> if k = m then true else gt k m
let (>) = gt
// equality by counting nest depth equality
let rec eq n m = match n, m with 
    | O, O -> true
    | O, Suc _ | Suc _, O -> false
    | Suc k, Suc l -> eq k l
let (=) = eq
// lazy def of less than
let lt (n:natural) (m:natural) = ~(n > m /\ n = m)  
let (<) = lt
// le and ge defs using lt, gt and eq
let le (n:natural) (m:natural) = n < m \/ n = m  
let (<=) = le  
let ge (n:natural) (m:natural) = n > m \/ n = m  
let (>=) = ge  
let nq (n:natural) (m:natural) = ~(n=m)
let (<>) = nq
// church notation of addition
let rec plus n m = match n with 
    | O -> m 
    | Suc k -> Suc (plus k m) 
let (+) = plus
// multiplication by repeating (+)
let rec mult n m = match n with 
    | O -> O 
    | Suc k -> plus m (mult k m)
let ( * ) = mult
// church notation of substraction
let rec minus (n:natural) (m:natural{ m<=n}) = match n, m with  
    | O , _ -> O
    | Suc _, O -> n
    | Suc k, Suc l -> minus k l
let ( -- ) = minus
// int power by repeating (*)
let rec pow  n m = match m with 
    | O -> Suc O
    | Suc k -> mult n (pow n k)
let (^) = pow
// get length of a list
let rec length l = match l  with 
    | Nil -> O
    | Cons h tl -> plus (Suc O) (length tl) 
// appends 2 lists
let rec append l r = match l with
    | Nil -> r 
    | Cons h tl -> Cons h (append tl r)


let _  = assert ((!0 = O)∧(!1 = Suc O));
         assert (!3  = Suc (Suc (Suc O)))            

let _  = assert (!2 + !3 = !5 /\ !5 -- !2 = !3);
         assert (!3 * !2 = !6 /\ !2  ^ !2 = !4)

let _  = assert (!2 = !2  /\ !2 <> !3);
         assert (!3 < !5  /\ !3  > !1);
         assert (!3 <= !3 /\ !3 <= !5);
         assert (!3 >= !3 /\ !5 >= !3)