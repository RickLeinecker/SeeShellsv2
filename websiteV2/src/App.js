import MenuBar from './components/MenuBar.js';
import FrontPage from './containers/FrontPage.js';
import AboutPage from './containers/AboutPage.js';
import DownloadPage from './containers/DownloadPage.js';
import DocumentationPage from './containers/DocumentationPage.js';
import DeveloperPage from './containers/DeveloperPage.js';
import Footer from './components/Footer.js';
import { withStyles } from '@material-ui/core/styles';
import { HashRouter as Router, Route } from 'react-router-dom';

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
    height: '100%',
    overflow: 'scroll',
  },
};

function App(props) {
  return (
    <div className={props.classes.application}>
      <Router basename="/">
        <div className={props.classes.topBar}>
          <MenuBar/>
        </div>
        <div className={props.classes.content}>
          <Route exact path="/">
            <FrontPage/>
          </Route>
          <Route exact path="/about">
            <AboutPage/>
          </Route>
          <Route exact path="/download">
            <DownloadPage/>
          </Route>
          <Route exact path="/documentation">
            <DocumentationPage/>
          </Route>
          <Route exact path="/developers">
            <DeveloperPage/>
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
