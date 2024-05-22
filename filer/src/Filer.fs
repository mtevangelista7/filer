module Filer

open System
open System.Text


// TODO: KKKKKK ok essas variáveis mutáveis são um problema, mas não consegui pensar em uma solução melhor então calma

/// e cria um loop que só finaliza quando o usuário digitar 'sair'
/// aqui lemos cada linha da entrada e concatenamos com a string do conteudo
/// após finalizar o loop, retornamos o novo conteudo
/// TODO: Verificar como eu posso remover essas variáveis mutáveis, não consegui pensar em uma solução aqui

let editLoop (content: string) =

    printfn "Modo edição, digite 'sair' em uma linha separada e pressione Enter para salvar e sair"

    let sb = new StringBuilder(content)

    // aqui temos duas variáveis mutáveis, uma para a posição do cursor e outra para continuar a edição
    let mutable cursorPosition = content.Length
    let mutable continueEditing = true

    // função que imprime o conteúdo do arquivo no console
    let printContent () =
        // aqui limpamos o console e imprimimos o conteúdo
        Console.Clear()
        Console.Write(sb.ToString())

        // aqui definimos a posição do cursor no console
        // a posição do cursor é controlada por coordenadas (x, y), onde x é a coluna e y é a linha
        //
        // cursorPosition representa a posição atual do cursor em termos de caracteres
        // Console.WindowWidth: largura da janela do console, ou seja, o número de colunas
        //
        // cursorPosition % Console.WindowWidth: o resto da divisão da posição do cursor pela largura da janela, isso calcula a posição horizontal do cursor dentro da linha atual
        // exemplo: se a largura da janela for 80 e a posição do cursor for 85, então 85 % 80 = 5, ou seja, o cursor está na coluna 5 da linha atual
        //
        // cursorPosition / Console.WindowWidth: a divisão inteira da posição do cursor pela largura da janela, isso calcula a linha atual do cursor
        Console.SetCursorPosition(cursorPosition % Console.WindowWidth, cursorPosition / Console.WindowWidth)

    // chamamos a função para imprimir o conteúdo
    printContent()

    // função recursiva que lê a entrada do usuário e realiza as operações de edição
    let rec loop () =

        // aqui lemos a tecla pressionada pelo usuário
        if continueEditing then

            // esse readkey(true) é para não mostrar a tecla pressionada no console
            let keyInfo = Console.ReadKey(true)

            // aqui realizamos o match da tecla pressionada
            match keyInfo.Key with

            // caso a tecla seja Backspace, removemos o caractere anterior
            | ConsoleKey.Backspace ->

                // verificamos se o cursor está na posição 0, se não estiver, removemos o caractere anterior
                if cursorPosition > 0 then

                    // removemos o caractere e decrementamos a posição do cursor
                    // aqui usamos |> ignore para ignorar o retorno da função, pois não precisamos dele, apenas queremos remover o caractere
                    sb.Remove(cursorPosition - 1, 1) |> ignore

                    // aqui, como a variável cursorPosition é mutável, podemos decrementá-la, <- é como se fosse um "="
                    cursorPosition <- cursorPosition - 1
                    printContent()

            // caso a tecla seja Enter, inserimos uma nova linha
            | ConsoleKey.Enter ->
                // aqui usamos |> ignore para ignorar o retorno da função, pois não precisamos dele, apenas queremos remover o caractere
                // TODO: esse \n ta funcionando no texto final mas no console fica todo bugado
                sb.Insert(cursorPosition, '\n') |> ignore

                // aqui, como a variável cursorPosition é mutável, podemos incrmenta-la, <- é como se fosse um "="
                cursorPosition <- cursorPosition + 1
                printContent()

            // caso a tecla seja Escape, finalizamos a edição
            | ConsoleKey.Escape ->
                continueEditing <- false

            // caso não seja nenhuma das teclas acima, inserimos o caractere pressionado e incrementamos a posição do cursor
            // para continuar a edição
            | _ ->
                // aqui usamos |> ignore para ignorar o retorno da função, pois não precisamos dele, apenas queremos remover o caractere
                sb.Insert(cursorPosition, keyInfo.KeyChar) |> ignore

                // aqui, como a variável cursorPosition é mutável, podemos incrmenta-la, <- é como se fosse um "="
                cursorPosition <- cursorPosition + 1
                printContent()
            loop()

    // chamamos a função loop para iniciar a edição
    loop()

    // retornamos o conteúdo do arquivo após a edição
    sb.ToString()

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
