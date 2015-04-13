/**
 * Created by aleksandrlazarenko on 12.04.15.
 */
$(function () {
    var nodes = [
        { id: 1, label: 'Коки', shape: 'circularImage', image: 'http://cocktion.com/Content/SiteImages/cocky.jpg' },
        { id: 2, label: 'Книга', shape: 'circularImage', image: 'http://cocktion.com/Content/SiteImages/book.jpg' },
        { id: 3, label: 'Услуга', shape: 'circularImage', image: 'http://cocktion.com/Content/SiteImages/service.jpg' },
        { id: 4, label: 'Вещь', shape: 'circularImage', image: 'http://cocktion.com/Content/SiteImages/stuff.png' }];

    // create an array with edges
    var edges = [
        { from: 1, to: 2 },
        { from: 1, to: 3 },
        { from: 1, to: 4 }
    ];

    // create a network
    var container = document.getElementById('mynetwork');
    var data = {
        nodes: nodes,
        edges: edges
    };
    var options = {
        width: '100%',
        height: '100%',
        physics: {
            barnesHut: {
                enabled: true,
                gravitationalConstant: -4000,
                springLength: 250
            }
        }
    };
    var network = new vis.Network(container, data, options);
    network.on('select', selectHandler);
})

//Скрывает дивы в зав-ти от того, какой попросят показать
function hideForms(formToShowName) {
    switch (formToShowName) {
        case 'cocky':
            $("#welcomeText").hide();
            $("#cockyText").show();
            $("#bookText").hide();
            $("#stuffText").hide();
            $("#serviceText").hide();
            break;
        case 'book':
            $("#welcomeText").hide();
            $("#cockyText").hide();
            $("#bookText").show();
            $("#stuffText").hide();
            $("#serviceText").hide();
            break;
        case 'stuff':
            $("#welcomeText").hide();
            $("#cockyText").hide();
            $("#bookText").hide();
            $("#stuffText").show();
            $("#serviceText").hide();
            break;
        case 'service':
            $("#welcomeText").hide();
            $("#cockyText").hide();
            $("#bookText").hide();
            $("#stuffText").hide();
            $("#serviceText").show();
            break;
    }
}

//Показывает ту или иную область на экране в зависимости от нажатого нодика
function selectHandler(properties) {
    var nodeId = properties.nodes[0];
    switch (nodeId) {
        case '1':
            hideForms('cocky');
            break;
        case '2':
            hideForms('book');
            break;
        case '3':
            hideForms('service');
            break;
        case '4':
            hideForms('stuff');
            break;
    }
}