import React from 'react';
import SideBar from '../components/SideBar.js';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';

const styles = {
    aboutPage: {
        display: 'flex',
        height: '100%',
        width: '100%',
    },
    sidebar: {
        float: 'left',
    },
};

class AboutPage extends React.Component {
    render() {
        return(
            <div className={this.props.classes.aboutPage}>
                <SideBar className={this.props.classes.sidebar}/>
            </div>
        );
    }
}

export default withStyles(styles)(withRouter(AboutPage));