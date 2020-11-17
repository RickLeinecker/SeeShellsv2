export default {
    beforeMount() {
        var url = this.$baseurl + 'SessionIsActive';

        var xhr = new XMLHttpRequest();
        xhr.open("GET", url, false);
        xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");
        xhr.setRequestHeader("X-Auth-Token", this.$session.get('session'));

        try {
            xhr.send(null);
            var result = JSON.parse(xhr.responseText);

            if (result.success != 1) {
                this.$session.destroy();
                this.$router.push('/SeeShells/login');
                location.reload();
            }
        }
        catch (err) {
            console.info(err);
        }
    }
};