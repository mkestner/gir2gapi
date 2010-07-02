ASSEMBLY = gir2gapi.exe
TARGETS = $(ASSEMBLY) $(ASSEMBLY).mdb
pkglib_DATA = $(TARGETS)
bin_SCRIPTS = gir2gapi

sources =				\
	Converter.cs			\
	#

DISTCLEANFILES = gir2gapi

EXTRA_DIST = $(sources) gir2gapi.in

CLEANFILES = $(TARGETS)

build_sources = $(addprefix $(srcdir)/, $(sources))

$(TARGETS): $(build_sources)
	$(CSC) -debug -out:$(ASSEMBLY) $(build_sources)

