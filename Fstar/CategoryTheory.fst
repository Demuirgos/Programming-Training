module CategoryTheory
//Note 2 Self : Needs finishing 
(*
    A Category is a Graph-like Concept, with Nodes 
    representing Objects and Edges representing 
    Morphisms (aka arrows), it obbeys a set of axioms :
    - the composability : f:(A->B) o g:(B->C) = h:(A->C)  
    - the identity      : f:(A->B) => Neutral<A> o f = f o Neutral<B>
*)

noeq type axioms (#object:Type) (arr : object -> object -> Type) 
                 (id:(o:object) -> arr o o) 
                 (compose:((#a:object) -> (#b:object) -> (#c:object) -> (arr a b) -> (arr b c) -> (arr a c))) = {
    neutrality    : #a:object -> #b:object 
                    -> f:arr a b -> Lemma ((compose (id a) f == f) /\ (compose f (id b) == f));
    associativity : #a:object -> #b:object -> #c:object -> #d:object
                    -> f:(arr a b) -> g:(arr b c) -> h:(arr c d) 
                    -> Lemma (compose f (compose g h) == compose (compose f g) h);
}

noeq type category (object:Type) (morphism: object -> object -> Type) : Type = 
    | Cat :  id     :(o:object -> morphism o o)
          -> compose: (#a:object -> #b:object -> #c:object 
                    -> (f:morphism a b) -> (g:morphism b c) -> (morphism a c))
          -> proofs : axioms morphism id compose
          -> category object morphism
    
