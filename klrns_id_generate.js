/**
 * This is an utility javascript function that can help you generate
 * custom id following the `KLRNS-<number>: <description>` convention
 * which is used for this project on `trello.com`.
 *
 * To use it go to trello, open console window and just copy paste
 * code in this file.
 */

/**
 * @returns {Array<string>} Sorted titles of all trello tasks that start with `KLRNS`.
 */
function getKlrnsArray() {
    let titles = document.querySelectorAll("span.list-card-title.js-card-name");
    let klrnsArray = [];
    titles.forEach(title => {
        const titleText = title.innerText;
        if (/^\s*?klrns/i.test(titleText)) {
            klrnsArray.push(titleText);
        }
    });
    klrnsArray.sort();
    return klrnsArray;
}

console.table(getKlrnsArray());