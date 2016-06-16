﻿namespace TokiwaDb.Core.Test

open Persimmon
open Persimmon.Syntax.UseTestNameByReflection
open TokiwaDb.Core

module SeqTest =
  let equalAllTest =
    parameterize {
      case (Seq.empty, true)
      case (seq [0], true)
      case (seq [0; 0; 0], true)
      case (seq [0; 0; 1], false)
      case (seq [1; 2; 3], false)
      run (Persimmon.testFun Seq.equalAll)
    }

module Int64Test =
  let ``toByteArray and ofByteArray Test`` =
    let f (n, bs) =
      test {
        do! n |> Int64.toByteArray |> assertEquals bs
        do! bs |> Int64.ofByteArray |> assertEquals n
      }
    parameterize {
      case (0L, Array.zeroCreate 8)
      case (0x1020304050ABCDEFL, [| 0x10uy; 0x20uy; 0x30uy; 0x40uy; 0x50uy; 0xABuy; 0xCDuy; 0xEFuy |])
      case (0xFFFFFFFFFFFFFFFFL, [| for i in 0..7 -> 0xFFuy |])
      run f
    }