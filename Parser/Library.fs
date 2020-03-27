namespace Parser

open System

type ExprType =
    | X
    | Const of value: float
    | Add of ExprType * ExprType
    | Sub of ExprType * ExprType
    | Mul of ExprType * ExprType
    | Div of ExprType * ExprType
    | Pow of ExprType * ExprType
    | Sin of ExprType
    | Cos of ExprType
    | Tan of ExprType
    | Ctg of ExprType
    | Abs of ExprType
    | Neg of ExprType

module Parser =
    let (|Digit|_|) = function
        | x::xs when Char.IsDigit(x) ->
            Some(Char.GetNumericValue(x), xs)
        | _ -> None
        
    let (|IntegerPart|_|) input =
        match input with
        | Digit(h, t) ->
            let rec loop acc = function
                | Digit(x, xs) -> loop ((acc * 10.) + x) xs
                | xs -> Some(acc, xs)
            loop 0. input
        | _ -> None
        
    let (|FractionalPart|_|) = function
        | '.'::t->
            let rec loop acc d = function
                | Digit(x, xs) -> loop ((acc * 10.) + x) (d * 10.) xs
                | xs -> (acc/d, xs)
            Some(loop 0. 1. t)
        | _ -> None

    let (|Number|_|) = function
        | IntegerPart(i, FractionalPart(f, t)) -> Some(i+f, t)
        | IntegerPart(i, t) -> Some(i, t)
        | FractionalPart(f, t) -> Some(f, t)
        | _ -> None
    let parse (expression) =
        let rec (|Expr|_|) = function
            | Mult(e, t) ->
                let rec loop l = function
                    | '+'::Mult(r, t) -> loop(Add(l, r)) t
                    | '-'::Mult(r, t) -> loop(Sub(l, r)) t
                    | [] -> Some(l, [])
                loop e t
        and (|Mult|_|) = function
            | Atom(l, '*'::Pwr(r, t)) -> Some(Mul(l, r), t)
            | Atom(l, '/'::Pwr(r, t)) -> Some(Div(l, r), t)
            | Pwr(e, t) -> Some(e, t)
            | _ -> None
        and (|Pwr|_|) = function
            | '+'::Atom(e, t) -> Some(e, t)
            | '-'::Atom(e, t) -> Some(Neg(e), t)
            | Atom(l, '^'::Pwr(r, t)) -> Some(Pow(l, r), t)
            | Atom(e, t) -> Some(e, t)
            | _ -> None
        and (|Atom|_|) = function
            | 'x'::t -> Some(X, t)
            | Number(e, t) -> Some(Const(e), t)
            | '('::Expr(e, ')'::t) -> Some(e, t)
            //| "sin("::Expr(e, ')'::t) -> Some(Sin(e), t)
            | _ -> None
            
        let parsed = (expression |> List.ofSeq |> (|Atom|_|))
        
        match parsed with
            | Some(result, _) -> result
            | None -> failwith "Failed to parse math expression"
