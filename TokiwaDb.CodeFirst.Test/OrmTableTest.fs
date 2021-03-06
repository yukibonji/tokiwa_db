namespace TokiwaDb.CodeFirst.Test

open TokiwaDb.CodeFirst
open Persimmon
open Persimmon.Syntax.UseTestNameByReflection

module OrmTableTest =
  open OrmDatabaseTest

  let insertTest =
    test {
      let db = testDb ()
      let person = Person(Name = "Miku", Age = 16L)
      db.Persons.Insert(person)
      do! person.Id |> assertEquals 0L
    }

  let seedDb () =
    let db = testDb ()
    db.Persons.Insert(Person(Name = "Miku", Age = 16L))
    db.Persons.Insert(Person(Name = "Yukari", Age = 18L))
    db.Songs.Insert(Song(Name = "Sayonara Chainsaw", Vocal = lazy "Yukari"))
    db

  let allItemsTest =
    test {
      let db = seedDb ()
      do! db.Persons.AllItems
        |> Seq.toArray
        |> Array.map (fun person -> person.Name)
        |> assertEquals [| "Miku"; "Yukari" |]
    }

  let lazyConstructTest =
    test {
      let db = seedDb ()
      let song = db.Songs.[0L]
      do! song.Vocal.IsValueCreated |> assertEquals false
      do! song.Vocal.Value |> assertEquals "Yukari"
    }

  let removeTest =
    test {
      let db = seedDb ()
      db.Persons.Remove(0L)
      do! db.Persons.Items
        |> Seq.map (fun person -> person.Name)
        |> Seq.toArray
        |> assertEquals [| "Yukari" |]
    }
