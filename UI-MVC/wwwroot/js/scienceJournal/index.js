const loadJournalsButton = document.getElementById('loadJournals');
const journalsTableBody = document.querySelector('#journalsTable tbody');

const countryOfOriginEnum = {
    0: "UNKNOWN",
    1: "USA",
    2: "UK",
    3: "Switzerland",
    4: "Germany"
};

window.addEventListener('load', loadJournals);
if (loadJournalsButton) {
    loadJournalsButton.addEventListener('click', loadJournals);
}

function loadJournals() {
    fetch('/api/ScienceJournals', {
        method: 'GET', headers: {'Accept': 'application/json'}
    })
        .then(response => {
            if (response.ok) return response.json();
        })
        .then(data => showJournals(data))
        .catch(err => alert(`Something went wrong.. :-/
            ${err.message}`));
}

function showJournals(journals) {
    journalsTableBody.innerHTML = '';
    journals.forEach(journal => addJournalToList(journal));
}

function addJournalToList(journal) {
    const row = document.createElement('tr');

    const nameCell = document.createElement('td');
    nameCell.textContent = journal.name;
    row.appendChild(nameCell);

    const priceCell = document.createElement('td');
    priceCell.textContent = journal.price == null ? "UNKNOWN" : "€"+journal.price.toFixed(2);
    row.appendChild(priceCell);
    
    const yearCell = document.createElement('td');
    yearCell.textContent = journal.yearFounded;
    row.appendChild(yearCell);

    const countryCell = document.createElement('td');
    countryCell.textContent = countryOfOriginEnum[journal.countryOfOrigin] || "Invalid Country Code";
    row.appendChild(countryCell);

    journalsTableBody.appendChild(row);
}
