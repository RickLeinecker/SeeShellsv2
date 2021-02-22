import React from 'react';
import SideBar from '../components/SideBar.js';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';

const styles = {
    documentationPage: {
        display: 'flex',
        height: '100%',
        width: '100%',
    },
    sidebar: {
        float: 'left',
    },
};

class DocumentationPage extends React.Component {
    render() {
        return(
            <div className={this.props.classes.documentationPage}>
                <SideBar className={this.props.classes.sidebar}/>
            </div>
        );
    }
}

export default withStyles(styles)(withRouter(DocumentationPage));