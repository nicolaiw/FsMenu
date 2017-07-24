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
    
    printfn "Use Keys: UP-Arrow DOWN-Arrow ENTER BACK-SPACE\n"

    let test =
        Menu [
            "Item 1" => (fun () -> printf "selected Item 1")
            "Item 2" +>
               Menu [ 
                   "Sub 1" +>
                           Menu [
                               "Sub Sub 1" => (fun () -> printf "selected Sub Sub 1")
                               "Sub Sub 2" => (fun () -> printf "selected Sub Sub 2")
                               "Sub Sub 3" <+ testFunc]
                   "Sub 2" => (fun () -> printf "selected Sub 2") ]]

    render test "<--"
    

    Console.ReadLine() |> ignore
    0
