open System
open Menu


[<EntryPoint>]
let main argv =
    
    printfn "Us Keys: UP-Arrow DOWN-Arrow ENTER BACK-SPACE\n"

    let test = Menu [
                     "Item 1" => (fun () -> printf "selected Item 1")
                     "Item 2" +>
                        Menu [ 
                            "Sub 1" +>
                                    Menu [
                                        "Sub Sub 1" => (fun () -> printf "selected Sub Sub 1")
                                        "Sub Sub 2" => (fun () -> printf "selected Sub Sub 2")
                                        "Sub Sub 3" => (fun () -> printf "selected Sub Sub 3")]
                            "Sub 2" => (fun () -> printf "selected Sub 2") ]]

    render test "<--"
    

    Console.ReadLine() |> ignore
    0
