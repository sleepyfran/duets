export enum DialogType {
    hide,
    databaseDownloadPrompt,
    databaseDownloadProgress,
}

export type UiState = {
    dialog: DialogType
}
