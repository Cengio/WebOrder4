Public Class intCanaleDirezione
    Public Property IntDirezione() As direzioneInterazione
    Public Property IntCanale() As canaleInterazione
End Class

Public Enum direzioneInterazione
    ENTRATA
    USCITA
    ENTRATAUSCITA
End Enum

Public Enum canaleInterazione
    TELEFONO
    EMAIL
    FAX
    POSTA
    WEB
    AGENTE
End Enum
