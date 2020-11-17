<template>
    <div id="team">
        <h1>Meet the Team</h1>
        <h4 class="section-subheading text-muted">The SeeShells application is a Senior Design project built by five Computer Science students at UCF.</h4>
        <div class="column">
            <div class="card">
                <img src="./../assets/logo.png" alt="Sara" style="width:100%">
                <div class="container">
                    <h2>Sara Frackiewicz</h2>
                    <p class="title">API, Scripting, and Administrative Website</p>
                </div>
            </div>
        </div>
        <div class="column">
            <div class="card">
                <img src="./../assets/logo.png" alt="Klayton" style="width:100%">
                <div class="container">
                    <h2>
                        Klayton Killough
                    </h2>
                    <p class="title">WPF Shellbag Parser and IO</p>
                </div>
            </div>
        </div>
        <div class="column">
            <div class="card">
                <img src="./../assets/logo.png" alt="Aleks" style="width:100%">
                <div class="container">
                    <h2>Aleksandar Stoyanov</h2>
                    <p class="title">WPF Shellbag Parser and Timeline</p>
                </div>
            </div>
        </div>
        <div class="column">
            <div class="card">
                <img src="./../assets/logo.png" alt="Bridget" style="width:100%">
                <div class="container">
                    <h2>Bridget Woodye</h2>
                    <p class="title">WPF GUI and Timeline</p>
                </div>
            </div>
        </div>
        <div class="column">
            <div class="card">
                <img src="./../assets/logo.png" alt="Yara" style="width:100%">
                <div class="container">
                    <h2>Yara As-Saidi</h2>
                    <p class="title">WPF and Website Content</p>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
    body {
        font-family: Arial, Helvetica, sans-serif;
        margin: 0;
    }

    html {
        box-sizing: border-box;
    }

    *, *:before, *:after {
        box-sizing: inherit;
    }

    .column {
        float: left;
        width: 33.3%;
        margin-bottom: 16px;
        padding: 0 8px;
    }

    .card {
        box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
        margin: 8px;
    }

    .container {
        padding: 0 16px;
    }

        .container::after, .row::after {
            content: "";
            clear: both;
            display: table;
        }

    .title {
        color: grey;
        text-align:center;
    }

    @media screen and (max-width: 650px) {
        .column {
            width: 100%;
            display: block;
        }
    }
</style>
<script>
    export default {
        name: 'team'
    }
</script>
