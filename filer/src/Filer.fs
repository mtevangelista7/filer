module Filer

open System

/// e cria um loop que só finaliza quando o usuário digitar 'sair'
/// aqui lemos cada linha da entrada e concatenamos com a string do conteudo
/// após finalizar o loop, retornamos o novo conteudo
/// TODO: Verificar como eu posso remover essas variáveis mutáveis, não consegui pensar em uma solução aqui
let editLoop (content: string) =
    printfn "Modo edição, digite 'sair' em uma linha separada"

    // printo o conteudo atual na mesma linha
    printf "%s" content

    // aqui definimos duas variáveis mutáveis, que controlam o conteúdo do texto e se o loop deve continuar
    let mutable newContent = content
    let mutable continueEditing = true

    // aqui temos o loop onde salvamos cada linha da entrada do usuario no novo conteudo
    while continueEditing do
        let line = Console.ReadLine()
        if line = "sair" then
            continueEditing <- false
        else
            newContent <- newContent + line + "\n"

    // retornando o conteudo
    newContent

/// função que salva o conteudo em um arquivo
let saveFile (filePath: string) (content: string) =
    System.IO.File.WriteAllText(filePath, content)

/// Aqui temos uma função recursiva (rec) que recebe um caminho de arquivo e o conteúdo do arquivo
let rec run(filePath: string) (content: string) =
    printfn "Digite 'salvar' para salvar, 'editar' para edição direta ou 'sair' para sair."

    // aqui realizamos a leitura da entrada do usuário e a separamos em 3 partes (comando, nome do arquivo e conteúdo)
    let input = Console.ReadLine().Split([|' '|], 3)

    // aqui realizamos o match do comando, e chamamos a função correspondente
    match input with
    | [| "salvar"; filePath; content |] ->
        saveFile filePath content
        run filePath content
    | [| "sair"; |] ->
        saveFile filePath content
        printfn "Saindo..."
    | [| "editar"; |] ->
        let newContent = editLoop content
        printfn "%s" newContent
        run filePath newContent

        // essa parte é o default, caso o comando não seja reconhecido
    | _ ->
        printfn "Comando inválido."
        run filePath content
