<template>
    <div id="app">
        <NavigationBar />

        <div id="page">
            <router-view />
        </div>
    </div>
</template>

<script>
    import NavigationBar from './components/NavigationBar.vue'
export default {
  name: 'App',
        components: {
          NavigationBar
        }
}
</script>

<style scoped>
    #page {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
        margin: 50px;
    }
</style>