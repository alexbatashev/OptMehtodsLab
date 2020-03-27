module ParserTest

open NUnit.Framework

open Parser 

[<SetUp>]
let Setup () =
    ()

[<Test>]
let Test1 () =
    Parser.parse "1 + 2.0"
    Parser.parse "x + (3 * x)"
    Assert.Pass()