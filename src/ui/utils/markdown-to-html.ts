import Showdown from 'showdown'

export default (markdown: string) => {
    const showdown = new Showdown.Converter()
    return showdown.makeHtml(markdown)
}
