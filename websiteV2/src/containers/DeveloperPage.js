import React from 'react';
import DeveloperProfile from '../components/DeveloperProfile.js';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';
import { Typography, Paper } from '@material-ui/core';

const styles = {
    devContainer: {
        display: 'flex',
        flexFlow: 'column',
        justifyContent: 'center',
        alignContent: 'center',
        width: '100%',
        overflow: 'auto',
    },
    devs: {
        display: 'flex',
        height: '100%',
        justifyContent: 'space-evenly',
        flexWrap: 'wrap',
        overflow: 'auto',
    },
    title: {
        fontSize: '50px',
        fontWeight: 'bold',
        marginTop: '1%',
        alignSelf: 'center',
        textAlign: 'center',
        color: '#33A1FD',
    },
    text: {
        textAlign: 'center',
    },
    video: {
        maxHeight: '360px',
        maxWidth: '640px',
        minHeight: '150px',
        minWidth: '300px',
        height: '50%',
        width: '50%',
        margin: '10px',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    },
};

/*
*   DeveloperPage.js
*   - renders all of the developer profiles and handles what content is passed into them
*   - version={1}: V1 team members, renders a card with the V1 logo
*   - version={2}: V2 team members, renders a card with the V2 logo
*   - version={0}: SeeShells sponsor, renders a card with the UCF pegasus logo
*   - name={}: the name to be rendered on the card
*   - role={}: the role to be rendered on the card
*/
class DeveloperPage extends React.Component {
    render() {
        return(
            <Paper className={this.props.classes.devContainer}>
                <Paper className={this.props.classes.devs}>
                    <Typography variant="title" className={this.props.classes.title}>SeeShells Development Teams</Typography>
                    <Typography variant="subtitle1" className={this.props.classes.text}>
                        The SeeShells application is a Senior Design project built by two teams of five Computer Science students at UCF.
                        The V1 team designed the original application, while the V2 team enhanced the project.
                    </Typography>
                    <DeveloperProfile version={1} name="Sara Frackiewicz" role="V1 Team: API, Scripting, and Administrative Website"/>
                    <DeveloperProfile version={1} name="Klayton Killough" role="V1 Team: WPF Shellbag Parser and IO"/>
                    <DeveloperProfile version={1} name="Aleksandar Stoyanov" role="V1 Team: WPF Shellbag Parser and Timeline"/>
                    <DeveloperProfile version={1} name="Bridget Woodye" role="V1 Team: WPF GUI and Timeline"/>
                    <DeveloperProfile version={1} name="Yara As-Saidi" role="V1 Team: WPF and Website Content"/>
                    <DeveloperProfile version={2} name="Devon Gadarowski" role="V2 Team: Shellbag Data Collection and Analysis"/>
                    <DeveloperProfile version={2} name="Kaylee Hoyt" role="V2 Team: Website Developer"/>
                    <DeveloperProfile version={2} name="Jake Meyer" role="V2 Team: WPF UI, Export Features"/>
                    <DeveloperProfile version={2} name="Spencer Ross" role="V2 Team: Team Manager, Application Testing and Backend Configuration"/>
                    <DeveloperProfile version={2} name="Joshua Rueda" role="V2 Team: Video Producer, Case Study Writer"/>
                    <DeveloperProfile version={0} name="Rick Leinecker" role="Project Sponsor"/>
                </Paper>
            </Paper>
        );
    }
}

export default withStyles(styles)(withRouter(DeveloperPage));