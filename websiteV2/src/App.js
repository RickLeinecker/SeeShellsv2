import MenuBar from './components/MenuBar.js';
import FrontPage from './containers/FrontPage.js';
import AboutPage from './containers/AboutPage.js';
import DownloadPage from './containers/DownloadPage.js';
import DocumentationPage from './containers/DocumentationPage.js';
import DeveloperPage from './containers/DeveloperPage.js';
import AdminLogin from './containers/AdminLogin.js';
import Footer from './components/Footer.js';
import { withStyles } from '@material-ui/core/styles';
import { BrowserRouter as Router, Route } from 'react-router-dom';

const styles = {
  application: {
    height: '100%',
    width: '100%',
    display: 'flex',
    flexDirection: 'column',
    flex: '1',
  },
  topBar: {
    color: 'white',
    display: 'flex',
    margin: '0px',
  },
  bottomBar: {
    color: 'white',
    display: 'flex',
    margin: '0px',
    width: '100%',
    marginTop: 'auto',
  },
  content: {
    color: 'black',
    display: 'flex',
    width: '100%',
  },
};

function App(props) {
  return (
    <div className={props.classes.application}>
      <Router>
        <div className={props.classes.topBar}>
          <MenuBar/>
        </div>
        <div className={props.classes.content}>
          <Route exact path="/">
            <FrontPage/>
          </Route>
          <Route path="/about">
            <AboutPage/>
          </Route>
          <Route path="/download">
            <DownloadPage/>
          </Route>
          <Route path="/documentation">
            <DocumentationPage/>
          </Route>
          <Route path="/developers">
            <DeveloperPage/>
          </Route>
          <Route path="/login">
            <AdminLogin/>
          </Route>
        </div>
        <div className={props.classes.bottomBar}>
          <Footer/>
        </div>
      </Router>
    </div>
  );
}

export default withStyles(styles)(App);
