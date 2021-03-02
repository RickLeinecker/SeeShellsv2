import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, HashRouter as Router } from 'react-router-dom';
import Paper from '@material-ui/core/Paper';
import Button from '@material-ui/core/Button';
import ButtonGroup from '@material-ui/core/ButtonGroup';

const styles = {
    aboutPage: {
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

class AboutPage extends React.Component {
    render() {
        return(
            <Router basename="/about">
                <Paper className={this.props.classes.aboutPage}>
                    <Paper elevation={2} square={true} className={this.props.classes.sidebarContainer}>
                        <Button className={this.props.classes.primaryButtons}>About</Button>
                        <ButtonGroup orientation="vertical">
                            <Button className={this.props.classes.buttons}>Windows Registry</Button>
                            <Button className={this.props.classes.buttons}>Shellbags</Button>
                        </ButtonGroup>
                        <Button className={this.props.classes.primaryButtons}>SeeShells</Button>
                        <ButtonGroup orientation="vertical">
                            <Button className={this.props.classes.buttons}>Parsing</Button>
                            <Button className={this.props.classes.buttons}>Analysis</Button>
                            <Button className={this.props.classes.buttons}>Timeline</Button>
                            <Button className={this.props.classes.buttons}>Filtering</Button>
                        </ButtonGroup>
                        <Button className={this.props.classes.primaryButtons}>Case Studies</Button>
                    </Paper>
                </Paper>
            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(AboutPage));