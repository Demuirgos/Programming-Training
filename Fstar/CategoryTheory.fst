module CategoryTheory
(*
    A Category is a Graph-like Concept, with Nodes 
    representing objs and Edges representing 
    Morphisms (aka arrows), it obbeys a set of axioms :
    - the composability : f:(A->B) o g:(B->C) = h:(A->C)  
    - the identity      : f:(A->B) => Id<A> o f = f o Id<B>
*)
noeq type axioms (#obj:Type) (arr : obj -> obj -> Type) 
                 (id:(o:obj) -> arr o o) 
                 (compose:((#a:obj) -> (#b:obj) -> (#c:obj) -> (arr a b) -> (arr b c) -> (arr a c))) = {
    neutrality    : #a:obj -> #b:obj 
                    -> f:arr a b -> Lemma (((id a) `compose` f == f) /\ (f `compose` (id b) == f));
    associativity : #a:obj -> #b:obj -> #c:obj -> #d:obj
                    -> f:(arr a b) -> g:(arr b c) -> h:(arr c d) 
                    -> Lemma (f `compose` (g `compose` h) == (f `compose` g) `compose` h);
}

noeq type category (obj:Type) (morphism: (obj -> obj -> Type)) : Type = 
    | Cat :  id     :(o:obj -> morphism o o)
          -> compose: (#a:obj -> #b:obj -> #c:obj 
                    -> (f:morphism a b) -> (g:morphism b c) -> (morphism a c))
          -> proofs : axioms morphism id compose
          -> category obj morphism
    
type le : nat -> nat -> Type =
    | LERflx : #n:nat -> le n n
    | LENext : #n:nat -> #m:nat -> le n m -> le n (m + 1)
let (<=) = le
let natPoset_le : category nat (<=) = admit()

type ge : nat -> nat -> Type =
    | GERflx : #n:nat -> ge n n
    | GENext : #n:nat -> #m:nat -> ge n m -> ge (n + 1) m
let (>=) = ge
let natPoset_ge : category nat (>=) = admit()