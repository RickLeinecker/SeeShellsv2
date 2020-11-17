<template>
    <div id="helpHeader">
        <h1>{{helpTitle}}</h1>
        <div id="helpContent">
            <VueShowdown :markdown="helptext"/>
        </div>
    </div>
</template>

<script>

    export default {
        name: 'HelpPage',
        data() {
            return {
                helpTitle: '',
                helptext: '<h5>Loading help...</h5>',
            }
        },
        methods: {
            populatePage() {
                var url = this.$baseurl + 'getHelpInformation';

                var xhr = new XMLHttpRequest();
                xhr.open("GET", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");

                try {
                    xhr.send(null);
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        this.helpTitle = result.json[0].title;
                        this.helptext = result.json[0].description;
                    }
                    else {
                        this.helptext = 'Failed to get the documentation!';
                    }

                }
                catch (err) {
                    console.info(err);
                }
            },
        },
        mounted() {
            this.$nextTick().then(this.populatePage);
        }
    }
</script>

<style scoped>
    #helpHeader {
        margin: 50px;
    }

    #helpContent {
        margin: auto;
        height: 100%;
        width: 90%;
        text-align: left;
    }
</style>