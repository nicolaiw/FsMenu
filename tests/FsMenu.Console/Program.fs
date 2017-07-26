open System
open Menu


let testFunc() = 
    printfn "selected Sub Sub 3"
    printf "handle some input: "
    let input = Console.ReadLine()
    // Do some stuff with input
    ()


[<EntryPoint>]
let main argv =
    
    (* 
        The ugly way
        don't use it
    *)

    let mutable yesOrNoUglyWay = ""

    let uglyTest =
                   Sub ([
                          ("Item 1", MenuEntry.Action (fun () -> printf "selected Item 1"; Exit))
                          ("Item 2", 
                            Sub ([
                                    ("Sub 1",
                                        Sub ([
                                                ("Sub Sub 1", MenuEntry.Action (fun () -> printf "selected Sub Sub 1"; Exit))
                                                ("Sub Sub 2",
                                                    Sub ([
                                                            ("yes", Action (fun () -> yesOrNoUglyWay <- "--yes"; NavigateBack))
                                                            ("no ", Action (fun () -> yesOrNoUglyWay <- "--no"; NavigateBack))
                                                        ]))
                                                ("Sub Sub 3", MenuEntry.Action (fun () -> testFunc(); NavigateBack))
                                             ]))
                                    ("Sub 2", MenuEntry.Action (fun () -> printf "selected Sub 2"; Exit))
                                    ("Sub 3", MenuEntry.Action (fun () -> printf "exec some command with param %s" yesOrNoUglyWay; Exit))
                                ]))
                       ]) 


    printfn "Use Keys: UP-Arrow DOWN-Arrow ENTER BACK-SPACE\n"

    (*
        The same using a small DSL
    *)

    let mutable yesOrNo = ""

    let test =
        Menu [
            "Item 1" => (fun () -> printf "selected Item 1")
            "Item 2" +>
               Menu [ 
                   "Sub 1" +>
                           Menu [
                               "Sub Sub 1" => (fun () -> printf "selected Sub Sub 1")
                               "Sub Sub 2" +>
                                            Menu [
                                                    "yes" <+ (fun () -> yesOrNo <- "--yes")
                                                    "no " <+ (fun () -> yesOrNo <- "--no") ]
                               "Sub Sub 3" <+ testFunc]
                   "Sub 2" => (fun () -> printf "selected Sub 2")
                   "Sub 3" => (fun () -> printf "exec some command with param %s" yesOrNo)]] 
                   

    render test "<--"
    

    Console.ReadLine() |> ignore
    0
