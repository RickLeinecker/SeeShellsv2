import MenuBar from './components/MenuBar.js';
import { withStyles } from '@material-ui/core/styles';

const styles = {
  root: {
    color: 'white',
    display: 'flex',
  }
};

function App(props) {
  return (
    <div className={props.classes.root}>
        <MenuBar/>
    </div>
  );
}

export default withStyles(styles)(App);
