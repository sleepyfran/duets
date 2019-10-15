export enum DialogType {
    Hide,
    DatabaseDownloadPrompt,
    DatabaseDownloadProgress,
    StartDateSelection,
}

export type UiState = {
    dialog: DialogType
}
