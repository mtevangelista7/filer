open System.IO
open Filer

// realiza a leitura de um arquivo
let readFile path =
    if System.IO.File.Exists(path) then
        let content = File.ReadAllText(path)
        content
    else
        ""

// o F# por padrão, possui um entry point implícito, porém nesse caso
// como queremos usar os args passados para utilizar a função run do filer
// devemos criar um entry point explicito
[<EntryPoint>]
let main argv =
  if argv.Length <> 1 then
    printfn "Use: filer <path>"
    1
  else
    let filePath = argv.[0]
    let content = readFile filePath
    if content <> "" then
        Filer.run filePath content
    0