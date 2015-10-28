OUTDIR=out
TEX=latexmk -xelatex -outdir=${OUTDIR}

all: proposal

.PHONY proposal:
	${TEX} proposal.tex
	cp ${OUTDIR}/proposal.pdf ./

clean:
	-@${RM} proposal.pdf
	-@${RM} ${OUTDIR}
