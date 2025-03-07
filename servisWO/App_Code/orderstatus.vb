
Module orderstatus

    ''' <summary>
    '''0 "SALVATO"
    '''1 "ATTESA PRODUZIONE"
    '''2 "INVIATO A MAGAZZINO"
    '''3 "IN LAVORAZIONE"
    '''4 "SPEDITO"
    '''5 "DA FATTURARE"
    '''6 "ANNULATO"
    '''7 "ATTESA APPROVAZIONE"
    ''' </summary>
    ''' <param name="statuscode"></param>
    ''' <param name="CompletedImported"></param>
    ''' <returns></returns>
    Function getDescription(ByVal statuscode As Integer, ByVal CompletedImported As Integer) As String
        Select Case statuscode
            Case 0
                If CompletedImported = 1 Then
                    Return "SPEDITO"
                Else
                    Return "SALVATO"
                End If
            Case 1
                Return "ATTESA PRODUZIONE"
            Case 2
                Return "INVIATO A MAGAZZINO"
            Case 3
                Return "IN LAVORAZIONE"
            Case 4
                If CompletedImported = 1 Then
                    Return "FATTURATO"
                Else
                    Return "SPEDITO"
                End If
            Case 5
                Return "DA FATTURARE"
            Case 6
                Return "ANNULATO"
            Case 7
                Return "ATTESA APPROVAZIONE"
            Case Else
                Return "N.D."
        End Select

    End Function


End Module
