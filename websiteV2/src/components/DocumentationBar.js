import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';
import Paper from '@material-ui/core/Paper';
import Button from '@material-ui/core/Button';
import ButtonGroup from '@material-ui/core/ButtonGroup';

const styles = {
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

class DocumentationBar extends React.Component {
    constructor(props) {
        super(props);

        this.handleClick = this.handleClick.bind(this);
    }

    handleClick(event) {
        if (event.currentTarget.id === "documentation") {
            this.props.history.push("/");
        } else {
            this.props.history.push("/" + event.currentTarget.id);
        }
    }

    render() {
        return(
            <Paper elevation={2} square={true} className={this.props.classes.sidebarContainer}>
                <Button className={this.props.classes.primaryButtons} onClick={this.handleClick} id="documentation">How to Use</Button>
                <ButtonGroup orientation="vertical">
                    <Button className={this.props.classes.buttons} onClick={this.handleClick} id="online">Online Parsing</Button>
                    <Button className={this.props.classes.buttons} onClick={this.handleClick} id="offline">Offline Parsing</Button>
                    <Button className={this.props.classes.buttons} onClick={this.handleClick} id="advanced">Advanced Configuration</Button>
                    <Button className={this.props.classes.buttons} onClick={this.handleClick} id="toolbar">Toolbar</Button>
                </ButtonGroup>
                <Button className={this.props.classes.primaryButtons} onClick={this.handleClick} id="data">Viewing the Data</Button>
                <ButtonGroup orientation="vertical">
                    <Button className={this.props.classes.buttons} onClick={this.handleClick} id="timeline">The Timeline</Button>
                    <Button className={this.props.classes.buttons} onClick={this.handleClick} id="events">Events</Button>
                    <Button className={this.props.classes.buttons} onClick={this.handleClick} id="filters">Filters</Button>
                    <Button className={this.props.classes.buttons} onClick={this.handleClick} id="export">Exporting</Button>
                    <Button className={this.props.classes.buttons} onClick={this.handleClick} id="import">Importing</Button>
                </ButtonGroup>
                <Button className={this.props.classes.primaryButtons} onClick={this.handleClick} id="licensing">Licensing</Button>
            </Paper>
        );
    }
}

export default withStyles(styles)(withRouter(DocumentationBar));