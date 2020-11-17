<template>
    <div id="osHeader">
        <h1>Add a new Windows OS Version</h1>
        <NewOS />
        <br />
        <div id="osContent">
            <ViewOSandFiles />
        </div>
    </div>
</template>

<script>
    import NewOS from './NewOS.vue';
    import ViewOSandFiles from './ViewOSandFiles.vue';
    import CheckIfAuthenticated from '../mixins/CheckIfAuthenticated';

    export default {
        name: 'OSPage',
        components: { NewOS, ViewOSandFiles },
        mixins: [CheckIfAuthenticated]
    }
</script>

<style scoped>
    #osHeader {
        margin: 50px;
    }

    #osContent {
        margin: auto;
        height: 100%;
        width: 70%;
    }
</style>