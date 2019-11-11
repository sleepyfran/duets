export const tryParseJson = (json: string): Promise<any> => {
    return new Promise((resolve, reject) => {
        try {
            return resolve(JSON.parse(json))
        } catch (err) {
            reject(err)
        }
    })
}
