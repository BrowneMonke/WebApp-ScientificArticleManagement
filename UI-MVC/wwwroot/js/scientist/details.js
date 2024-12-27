const articleFeaturesSection = document.getElementById('articleFeatures');
const articlesTable = document.createElement('table');
const articlesTableBody = document.createElement('tbody');

const categoryEnum = {
    0: "Astrophysics",
    1: "Neuroscience",
    2: "Marine Ecology",
    3: "Quantum Physics",
    4: "Electromagnetism",
    5: "Chemistry",
    6: "Biology"
};

window.addEventListener('load', init);

function init() {
    loadStartingElements();
    loadArticlesOfScientist();
    loadArticleAssignmentForm();
}

function loadStartingElements() {
    const allArticlesHeader = document.createElement('h3');
    allArticlesHeader.textContent = "Articles";
    articleFeaturesSection.appendChild(allArticlesHeader);

    articlesTable.classList.add("table");
    articlesTable.innerHTML = `
        <thead>
        <tr>
            <th>Title</th>
            <th>Published</th>
            <th># Pages</th>
            <th>Category</th>
        </tr>
        </thead>
        `;
}

function loadArticlesOfScientist() {
    const scientistId = Number(document.getElementById('scientistIdentification').innerText);

    fetch('/api/ScientificArticles?scientistId='+scientistId, {
        method: 'GET', headers: {'Accept': 'application/json'}
    })
        .then(response => {
            if (response.ok) return response.json();
        })
        .then(data => showArticles(data))
        .catch(err => alert(`Something went wrong.. :-/
            ${err.message}`));
}

function showArticles(articles) {
    articlesTableBody.innerHTML = '';
    articles.forEach(art => addArticleToList(art));
    articlesTable.appendChild(articlesTableBody);
    articleFeaturesSection.appendChild(articlesTable);
}

function addArticleToList(article) {
    const row = document.createElement('tr');

    const titleCell = document.createElement('td');
    titleCell.textContent = article.title;
    row.appendChild(titleCell);

    const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    const dopCell = document.createElement('td');
    const dopValue = new Date(article.dateOfPublication);
    dopCell.textContent = `${months[dopValue.getMonth()]} ${dopValue.getDate()}, ${dopValue.getFullYear()}`;
    row.appendChild(dopCell);

    const numOfPagesCell = document.createElement('td');
    numOfPagesCell.textContent = Number(article.numberOfPages);
    row.appendChild(numOfPagesCell);

    const categoryCell = document.createElement('td');
    categoryCell.textContent = categoryEnum[article.category] || "Invalid Category Code";
    row.appendChild(categoryCell);

    articlesTableBody.appendChild(row);
}

function loadArticleAssignmentForm(){
    const addArticleHeader = document.createElement('h3');
    addArticleHeader.textContent = "Articles";
    articleFeaturesSection.appendChild(addArticleHeader);
    const addArticleFormElem = document.createElement('form')
}

function createAddArticleForm(addArticleFormElem) {
    addArticleFormElem.classList.add('form');
}
