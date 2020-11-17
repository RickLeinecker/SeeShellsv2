import Home from './components/Home.vue';
import Register from './components/RegisterForm.vue';
import TeamMembers from './components/TeamMembers.vue';
import About from './components/About.vue';
import Login from './components/LoginForm.vue';
import Scripts from './components/ScriptPage.vue';
import ApproveUsers from './components/ApproveUserPage.vue';
import GUIDPage from './components/GUIDPage.vue';
import OSPage from './components/OSPage.vue';
import ModifyOSPage from './components/ModifyOSPage.vue';
import HelpPage from './components/HelpPage.vue';
import UpdateHelpPage from './components/UpdateHelpPage.vue';

const root = '/SeeShells/'

const routes = [
    { path: root, component: Home },
    { path: root + 'register', component: Register },
    { path: root + 'team', component: TeamMembers },
    { path: root + 'about', component: About},
    { path: root + 'login', component: Login },
    { path: root + 'scripts', component: Scripts },
    { path: root + 'approveusers', component: ApproveUsers },
    { path: root + 'guids', component: GUIDPage },
    { path: root + 'os', component: OSPage },
    { path: root + 'modifyos', component: ModifyOSPage },
    { path: root + 'help', component: HelpPage },
    { path: root + 'updatehelp', component: UpdateHelpPage }
];

export default routes;