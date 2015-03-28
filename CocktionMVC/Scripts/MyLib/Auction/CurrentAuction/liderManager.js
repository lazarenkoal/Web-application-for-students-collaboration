function printLider(nodeId) {
    if (nodeId != null) {
        $('#leaderInfo').empty();
        $('#leaderInfo').append('В данный момент id лидера = ' + nodeId);
    }
}