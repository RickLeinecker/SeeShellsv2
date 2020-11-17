<template>
    <div id="nav">
        <b-navbar toggleable="lg" type="dark" variant="dark">
            <img alt="Vue logo" src="./../assets/logo.png" width="75" height="75">
            <router-link to='/SeeShells/'><b-navbar-brand>SeeShells</b-navbar-brand></router-link>

            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

            <b-collapse id="nav-collapse" is-nav>
                <router-link to='/SeeShells/about' tag="menuitem">About</router-link>
                <router-link to='/SeeShells/help' tag="menuitem">Documentation</router-link>
                <router-link to='/SeeShells/team' tag="menuitem">Developer Team</router-link>
                <b-navbar-nav>
                    <b-nav-item href="https://github.com/RickLeinecker/SeeShells">GitHub Page</b-nav-item>
                </b-navbar-nav>

                <!-- Right aligned nav items -->
                <b-navbar-nav class="ml-auto">
                    <b-nav-item-dropdown right>
                        <template v-slot:button-content>
                            <em>Administrative</em>
                        </template>
                        <div v-if="sessionExists">
                            <LoggedIn></LoggedIn>
                        </div>
                        <div v-else>
                            <LoggedOut></LoggedOut>
                        </div>
                    </b-nav-item-dropdown>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>
    </div>
</template>

<script>
    import LoggedIn from './AdministrativeLoggedIn.vue';
    import LoggedOut from './AdministrativeLoggedOut.vue';

    export default {
        name: 'NavigationBar',
        computed: {
            sessionExists() { // define it here to make it reactive
                return this.$session.exists();
            }         
        },
        components: { LoggedIn, LoggedOut },
    }

   
</script>

<style scoped>
    #nav{
          margin-left: 0px;
          margin-right: 0px;
    }
    nav menuitem:hover,
    nav menuitem:active{
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #CCCCCC;
    }
    nav menuitem{
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #a1a1a1;
        margin: 5px;
    }
</style>
