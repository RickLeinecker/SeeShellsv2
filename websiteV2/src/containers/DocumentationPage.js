import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';
import Paper from '@material-ui/core/Paper';
import Button from '@material-ui/core/Button';
import ButtonGroup from '@material-ui/core/ButtonGroup';

const styles = {
    documentationPage: {
        display: 'flex',
        height: '100%',
        width: '100%',
    },
    sidebarContainer: {
        width: '20%',
        height: '100%',
        backgroundColor: '#424242',
        margin: '0px',
        display: 'flex',
        flexDirection: 'column',
    },
    primaryButtons: {
        display: 'flex',
        justifyContent: 'left',
        color: '#F2F2F2',
        '&:hover': {
            color: '#33A1FD',
        },
        fontSize: '30px',
        fontFamily: 'Georgia',
    },
    buttons: {
        display: 'flex',
        justifyContent: 'center',
        color: '#F2F2F2',
        '&:hover': {
            color: '#33A1FD',
        },
        fontSize: '20px',
        fontFamily: 'Georgia',
    },
};

class DocumentationPage extends React.Component {
    render() {
        return(
            <Paper className={this.props.classes.documentationPage}>
                <Paper elevation={2} square={true} className={this.props.classes.sidebarContainer}>
                    <Button className={this.props.classes.primaryButtons}>How to Use</Button>
                    <ButtonGroup orientation="vertical">
                        <Button className={this.props.classes.buttons}>Online Parsing</Button>
                        <Button className={this.props.classes.buttons}>Offline Parsing</Button>
                        <Button className={this.props.classes.buttons}>Advanced Configuration</Button>
                        <Button className={this.props.classes.buttons}>Toolbar</Button>
                    </ButtonGroup>
                    <Button className={this.props.classes.primaryButtons}>Viewing the Data</Button>
                    <ButtonGroup orientation="vertical">
                        <Button className={this.props.classes.buttons}>The Timeline</Button>
                        <Button className={this.props.classes.buttons}>Events</Button>
                        <Button className={this.props.classes.buttons}>Filters</Button>
                        <Button className={this.props.classes.buttons}>Exporting</Button>
                        <Button className={this.props.classes.buttons}>Importing</Button>
                    </ButtonGroup>
                    <Button className={this.props.classes.primaryButtons}>Licensing</Button>
                </Paper>
            </Paper>
        );
    }
}

export default withStyles(styles)(withRouter(DocumentationPage));