<template>
    <div id="about">
        <h1>About SeeShells</h1>
        <p align="left" style="font-size:large">
            <br />
            SeeShells is an information extraction software. The objective is to
            create a standalone open source executable that can run both online and offline.
            It extracts and parses through Windows Registry information. This data
            is then converted into two forms. The first is a csv file that contains all the raw data we obtain from the registry.
            The second is a human readable timeline that can be downloaded and used as evidence in a courtroom.
            The timeline provides an interactive easier to read visualization of the data extracted from the
            windows registries. This information is otherwise difficult and time consuming to comb through and understand.
            The timeline can be filtered by date, event name, the contents of the event (e.g. accessed, modified, created),
            user, and the event type. These filters can be applied to all events and cleared out individually as the users see fit.
            The application also contains an about page as well as a help page so that users who are not able to connect to the internet are
            still able to use the program and obtain guidance if the need it.
        </p>
        <p align="left" style="font-size:large">
            The parsing and extraction of information has a slightly different
            process for each of the windows versions including Windows XP, Windows Vista
            Windows 7,8,8.1 and 10. In order to create a robust application we have set up a
            server to store database information on parsing different registry versions.
            We have implemented the use of embedded scripting in order to
            keep the application up-to-date without requiring the users to update the program or redownload it.
            Currently, we do not know all there is to know about shellbags. Currently unidentifiable shellbag
            items check if a script exists to parse it.

        </p>
        <p align="left" style="font-size:large">
            The software expediates the process of extracting, parsing, and presenting the
            registry information in a way that is condensed and easily understandable.
            We hope others will benefit from our interactive timeline generated from the ShellBag
            information and we hope to make a great impact on the digital forensics community.
            <br />
            <br />
        </p>
        <p />
    </div>
</template>

<script>
    export default {
        name: 'about'
    }
</script>


<style>
    #about {
        margin:auto;
        height: 100%;
        width: 90%;
    }
</style>