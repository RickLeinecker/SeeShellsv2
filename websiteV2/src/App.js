import MenuBar from './components/MenuBar.js';
import FrontPage from './containers/FrontPage.js';
import AdminPage from './containers/AdminPage.js';
import { withStyles } from '@material-ui/core/styles';
import { BrowserRouter as Router, Route } from 'react-router-dom';

const styles = {
  root: {
    color: 'white',
    display: 'flex',
    margin: '0px',
  },
  content: {
    color: 'black',
  },
};

function App(props) {
  return (
    <div>
      <Router>
        <div className={props.classes.root}>
          <MenuBar/>
        </div>
        <div className={props.classes.content}>
          <Route exact path="/">
            <FrontPage/>
          </Route>
          <Route path="/admin">
            <AdminPage/>
          </Route>
        </div>
      </Router>
    </div>
  );
}

export default withStyles(styles)(App);
