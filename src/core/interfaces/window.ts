export default interface WindowInteractor {
    exit: () => void
    openInBrowser: (url: string) => void
}
