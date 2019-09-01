export enum DialogType {
    Hide,
    DatabaseDownloadPrompt,
    DatabaseDownloadProgress,
}

export type UiState = {
    dialog: DialogType
}
